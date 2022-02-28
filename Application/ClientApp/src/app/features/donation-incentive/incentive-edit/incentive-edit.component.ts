import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FontList } from '@shared/typography/font-list';
import { TypographyStyles } from '@shared/typography/typography-styles';

import { IDonationIncentive } from '../dto/donation-incentive.interface';
import { FontPreferenceService } from '../services/font-preference.service';

interface EditIncentiveInputData {
  incentive: IDonationIncentive;
}

@Component({
  selector: 'pb-incentive-edit',
  templateUrl: './incentive-edit.component.html',
  styleUrls: ['./incentive-edit.component.scss'],
})
export class IncentiveEditComponent {
  fontsAvailable = FontList;
  fontStylesAvailable = TypographyStyles;

  incentive: IDonationIncentive;

  typographyFormGroup: FormGroup;
  incentiveFormGroup: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: EditIncentiveInputData,
    private dialogRef: MatDialogRef<IncentiveEditComponent>,
    private fontPreferenceService: FontPreferenceService
  ) {
    this.incentive = data.incentive;

    const preference = this.fontPreferenceService.getFontPreference();
    const { font, sizeInPx, style } = preference;

    this.incentiveFormGroup = new FormGroup({
      name: new FormControl(this.incentive.name, Validators.required),
      amount: new FormControl(this.incentive.amount, Validators.required),
      goal: new FormControl(this.incentive.goal, Validators.required),
    });

    this.typographyFormGroup = new FormGroup({
      font: new FormControl(font, Validators.required),
      size: new FormControl(sizeInPx, Validators.required),
      style: new FormControl(style, Validators.required),
    });
  }

  save() {
    this.saveFontPreference();
    const { name, amount, goal } = this.incentiveFormGroup.value;
    const newIncentive: IDonationIncentive = {
      name,
      amount,
      goal,
    };

    this.dialogRef.close(newIncentive);
  }

  private saveFontPreference() {
    const { font, size, style } = this.typographyFormGroup.value;
    this.fontPreferenceService.setFontPreference({
      font,
      sizeInPx: size,
      style,
    });
  }
}
