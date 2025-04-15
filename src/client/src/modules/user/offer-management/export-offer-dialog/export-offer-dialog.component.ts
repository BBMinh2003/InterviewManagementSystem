import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-export-dialog',
  imports: [CommonModule, FormsModule],
  templateUrl: './export-offer-dialog.component.html',
})
export class ExportOfferDialogComponent {
  visible = false;
  fromDate?: string;
  toDate?: string;
  errorMessage?: string;

  @Output() exportConfirmed = new EventEmitter<{ from: Date; to: Date }>();

  get isValid(): boolean {
    return !!this.fromDate && !!this.toDate && !this.errorMessage;
  }

  open() {
    this.visible = true;
  }

  close() {
    this.visible = false;
    this.reset();
  }

  export() {
    if (this.validateDates()) {
      this.exportConfirmed.emit({
        from: new Date(this.fromDate!),
        to: new Date(this.toDate!)
      });
      this.close();
    }
  }

  protected validateDates(): boolean {
    if (!this.fromDate || !this.toDate) return false;
    
    const from = new Date(this.fromDate);
    const to = new Date(this.toDate);
    
    if (from > to) {
      this.errorMessage = "End date must be after start date";
      return false;
    }
    
    this.errorMessage = undefined;
    return true;
  }

  private reset() {
    this.fromDate = undefined;
    this.toDate = undefined;
    this.errorMessage = undefined;
  }
}