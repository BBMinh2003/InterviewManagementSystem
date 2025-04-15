import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface ConfirmDialogData {
  title?: string;
  message?: string;
  confirmText?: string;
  cancelText?: string;
  confirmColor?: string;
  cancelColor?: string;
}

@Component({
  template: `
    <div class="p-6 max-w-md">
      <h2 class="text-xl font-semibold mb-4 text-gray-800">
        {{ data.title || 'Confirm Action' }}
      </h2>

      <p class="mb-6 text-gray-600">
        {{ data.message || 'Are you sure you want to perform this action?' }}
      </p>

      <div class="flex justify-end gap-3">
        <button
          class="px-4 py-2 rounded-md transition-colors {{
            data.cancelColor || 'bg-gray-100 hover:bg-gray-200 text-gray-700'
          }}"
          (click)="onNoClick()"
        >
          {{ data.cancelText || 'Cancel' }}
        </button>

        <button
          class="px-4 py-2 rounded-md transition-colors text-white {{
            data.confirmColor || 'bg-red-600 hover:bg-red-700'
          }}"
          (click)="onYesClick()"
        >
          {{ data.confirmText || 'Confirm' }}
        </button>
      </div>
    </div>
  `,
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData
  ) {}

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onYesClick(): void {
    this.dialogRef.close(true);
  }
}
