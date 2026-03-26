import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PilotFlowApiService } from '../../core/pilotflow-api.service';
import { CreateAccessRequestResponse } from '../../core/api.models';

@Component({
  selector: 'app-workflows-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './workflows.page.html',
  styleUrl: './workflows.page.scss'
})
export class WorkflowsPageComponent {
  private readonly api = inject(PilotFlowApiService);
  private readonly fb = inject(FormBuilder);

  submitting = false;
  errorMessage = '';
  result?: CreateAccessRequestResponse;

  readonly form = this.fb.nonNullable.group({
    tenantId: ['tenant-demo', Validators.required],
    requesterName: ['', Validators.required],
    requesterEmail: ['', [Validators.required, Validators.email]],
    systemName: ['', Validators.required],
    accessLevel: ['Read', Validators.required],
    managerName: ['', Validators.required],
    reason: ['', [Validators.required, Validators.maxLength(500)]]
  });

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting = true;
    this.errorMessage = '';
    this.result = undefined;

    this.api.createAccessRequest(this.form.getRawValue()).subscribe({
      next: (response) => {
        this.submitting = false;
        this.result = response;
      },
      error: (error) => {
        this.submitting = false;
        this.errorMessage = this.mapError(error);
      }
    });
  }

  isInvalid(controlName: string) {
    const control = this.form.get(controlName);
    return !!control && control.invalid && (control.touched || control.dirty);
  }

  private mapError(error: unknown): string {
    if (typeof error === 'object' && error && 'error' in error) {
      const body = (error as { error?: { message?: string } }).error;
      if (body?.message) {
        return body.message;
      }
    }

    return 'Unable to submit the request. Please try again.';
  }
}
