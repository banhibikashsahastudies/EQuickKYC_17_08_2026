const DEFAULT_STEPS = ["Basic", "Trading", "Bank", "Personal", "Nominee", "Documents"];

/**
 * Horizontal progress stepper for the multi-step KYC flow.
 *
 * `currentStep` is 1-indexed: steps before it render as completed (green,
 * checkmark), the step itself renders as active (blue), and steps after it
 * render as upcoming (grey). A step automatically turns green the moment
 * `currentStep` moves past it, so callers just advance the number as the
 * user completes each step.
 */
function KycStepper({ currentStep, steps = DEFAULT_STEPS }) {
  return (
    <div className="kyc-stepper" role="list" aria-label="KYC verification progress">
      {steps.map((label, index) => {
        const stepNumber = index + 1;
        const status =
          stepNumber < currentStep ? "completed" : stepNumber === currentStep ? "active" : "upcoming";
        const isLast = stepNumber === steps.length;

        return (
          <div className="kyc-step-item" key={label}>
            <div className="kyc-step" role="listitem" aria-current={status === "active" ? "step" : undefined}>
              <div className={`kyc-step-circle ${status}`}>
                {status === "completed" ? (
                  <i className="fa-solid fa-check" aria-hidden="true"></i>
                ) : (
                  stepNumber
                )}
              </div>
              <span className={`kyc-step-label ${status}`}>{label}</span>
            </div>
            {!isLast && <div className={`kyc-step-line ${status === "completed" ? "completed" : ""}`}></div>}
          </div>
        );
      })}
    </div>
  );
}

export default KycStepper;
