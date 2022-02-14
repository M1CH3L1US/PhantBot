using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core.Configuration;

[AttributeUsage(AttributeTargets.Class)]
public class ValidateConfigurationAttribute : Attribute {
}

[Generator]
public class ConfigurationValidationGenerator : ISourceGenerator {
  public void Initialize(GeneratorInitializationContext context) {
    context.RegisterForSyntaxNotifications(() => new ValidateConfigurationAttributeSyntaxReceiver());
  }

  public void Execute(GeneratorExecutionContext context) {
    var receiver = context.SyntaxReceiver as ValidateConfigurationAttributeSyntaxReceiver;
    var classDeclarations = receiver!.ClassesWithValidationAttribute;

    foreach (var declaration in classDeclarations) {
      if (!declaration.Members.Any()) {
        continue;
      }

      var declarationName = declaration.Identifier.Text;
      var generatedName = $"{declarationName}Validator";
      context.AddSource(generatedName, $"namespace Core.Configuration; class {generatedName} {{}}");
    }
  }


  public class ValidateConfigurationAttributeSyntaxReceiver : ISyntaxReceiver {
    public List<ClassDeclarationSyntax> ClassesWithValidationAttribute { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode) {
      if (syntaxNode is not ClassDeclarationSyntax classDeclaration) {
        return;
      }

      if (classDeclaration.AttributeLists.Count > 0) {
        return;
      }

      var hasAttribute = classDeclaration.AttributeLists.Any(HasValidationAttribute);


      if (hasAttribute) {
        ClassesWithValidationAttribute.Add(classDeclaration);
      }
    }

    private static bool HasValidationAttribute(AttributeListSyntax attributeList) {
      return attributeList.Attributes
                          .Any(
                            e => e.Name.ToFullString() == nameof(ValidateConfigurationAttribute)
                          );
    }
  }
}