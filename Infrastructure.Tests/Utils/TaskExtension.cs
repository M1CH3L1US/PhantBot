using System.Threading.Tasks;

namespace Infrastructure.Tests.Utils;

public static class TaskExtension {
  public static T ToSynchronous<T>(this Task<T> task) {
    task.Wait();
    return task.Result;
  }
}