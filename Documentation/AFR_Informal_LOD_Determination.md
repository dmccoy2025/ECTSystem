# Air Force Reserve (AFR) Informal LOD Determination

Based on the DAFI 36-2910 document (reissued November 12, 2024) and the tree view provided earlier, I will focus on the **Air Force Reserve (AFR) Informal LOD Determination** workflow, including its sub-workflows for **Appeals** and **Reinvestigations**. This response merges the workflow details with corresponding state machine terminology (states, transitions, triggers, guards, and actions) to align with the Stateless implementation discussed previously. It excludes Regular Air Force/Space Force (RegAF/USSF), Air National Guard (ANG), and the combined Air Reserve Component (ARC) section as requested.

## Methodology
- **Component**: Focus solely on Air Force Reserve (AFR).
- **Type**: Limit to LOD Determination, specifically the Informal LOD Determination workflow.
- **Sub-Workflows**: Include Appeals and Reinvestigations as sub-processes, as outlined in Chapters 3 and 5 (e.g., 3.2.2.9.3, 5.7.3).
- **State Machine Terminology**: Define **states** (distinct phases), **transitions** (movements between states), **triggers** (events causing transitions), **guards** (conditions for transitions), and **actions** (tasks during entry/exit) to map the workflow to a state machine model.
- **Source**: Use the tree view, document sections (Chapters 3–7, Tables 3.1–3.3), and guidance memorandum changes (e.g., timeline increase to 90 days).

## Air Force Reserve (AFR) Informal LOD Determination

- **Informal LOD Determination**
  - **Description**: A standard process for AFR members to determine if an illness, injury, disease, or death occurred in the line of duty, presuming "in line of duty" unless evidence suggests otherwise. Utilizes AF Form 348 and allows an interim LOD valid for 90 days. This workflow is tailored for AFR personnel assigned to or training with AFR units (CAT A).
  - **State: Initiated**
    - **Description**: Initial state where the member reports the condition.
    - **Transition**: MemberReportComplete → MedicalAssessment
    - **Trigger**: CompleteMedical (member reports to RMU and Immediate Commander within 5 days).
    - **Guard**: Member provides records within 5 calendar days.
    - **Entry Action**: Log initiation and notify RMU.
    - **Exit Action**: Route records to LOD-MFP.
  - **State: MedicalAssessment**
    - **Description**: State where the Military Medical Provider assesses the condition.
    - **Transition**: MedicalAssessmentComplete → Routing
    - **Trigger**: Route (LOD-MFP initiates and routes within 5 days, provider completes within 30 days or next UTA, 5 days if full-time).
    - **Guard**: Assessment completed within timeline (30/5 days based on provider availability).
    - **Entry Action**: Notify provider to assess condition.
    - **Exit Action**: Forward completed medical portion to Commander.
  - **State: Routing**
    - **Description**: State where the LOD-MFP and LOD PM handle document routing.
    - **Transition**: RoutingComplete → CommanderReview
    - **Trigger**: CompleteCommander (documents routed to Commander within workflow).
    - **Guard**: Documents received by Commander within 10 days of routing.
    - **Entry Action**: LOD PM routes documents.
    - **Exit Action**: Notify Commander for review.
  - **State: CommanderReview**
    - **Description**: State where the Immediate or first full-time Commander reviews and recommends.
    - **Transition**: CommanderReviewComplete → LegalReview
    - **Trigger**: CompleteLegal (Commander recommends within 10 days, optional SJA review in 5 days).
    - **Guard**: Recommendation submitted within 10 days (or 15 with SJA).
    - **Entry Action**: Commander gathers information.
    - **Exit Action**: Send recommendation to Legal Advisor (if opted).
  - **State: LegalReview**
    - **Description**: State where the Wing Staff Judge Advocate (optional) reviews for legal sufficiency.
    - **Transition**: LegalReviewComplete → AuthorityAdjudication
    - **Trigger**: Adjudicate (Legal review completed within 5 days if opted, otherwise skipped).
    - **Guard**: Legal sufficiency confirmed (optional step).
    - **Entry Action**: Legal Advisor checks sufficiency.
    - **Exit Action**: Forward to Wing Commander.
  - **State: AuthorityAdjudication**
    - **Description**: State where the Wing Commander and ARC LOD Board A1 adjudicate.
    - **Transition**: AdjudicationComplete → Finalized
    - **Trigger**: Finalize (Wing Commander reviews in 5 days, Board approves in 30 days, LOD PM notifies in 1 day).
    - **Guard**: Approval within 35 days total from Wing Commander to Board.
    - **Entry Action**: Authorities begin adjudication.
    - **Exit Action**: Notify member of final determination.
  - **State: Finalized**
    - **Description**: Terminal state where the LOD determination is complete.
    - **Transition**: AppealSubmitted → Appealed | VoidTriggered → Voided
    - **Trigger**: Appeal (within 30 days) or Void (after 90 days for interim).
    - **Guard**: Appeal submitted within 30 days or interim expires.
    - **Entry Action**: Distribute LOD package to member, alert of appeal rights.
    - **Exit Action**: Log final status (awaiting appeal or void).
  - **State: Voided**
    - **Description**: Terminal state where the process is voided (e.g., after 90 days for interim LOD).
    - **Transition**: None (terminal state).
    - **Trigger**: N/A
    - **Guard**: N/A
    - **Entry Action**: Log void status and close case.
    - **Exit Action**: N/A
  - **Timeline**: 90 calendar days total, reflecting the increase from 60 days per DAFGM2023-02.
  - **Authorities**: 
    - Immediate Commander (first full-time CC in chain).
    - Senior AFR CC present (appointing/reviewing).
    - AFRC LOD Determination Board (approving).
    - HQ AFRC/A1 (adjudicating informal LODs).
    - HQ AFRC/CD (appellate authority).
  - **Notes**: The interim LOD determination is valid for 90 days and void upon completion of the finalized LOD. Quarterly audit reports are submitted to AFRC/CD, per 3.2.2.9.3.

  - **Sub-Workflow: Appeals**
    - **Description**: Allows the member to challenge the final Informal LOD determination if dissatisfied.
    - **State: Appealed**
      - **Description**: State where the appeal process is initiated.
      - **Transition**: AppealReviewComplete → AuthorityAdjudication | AppealResolved → Finalized
      - **Trigger**: Adjudicate (re-review by Appellate Authority) or Finalize (appeal resolved).
      - **Guard**: Appeal submitted within 30 calendar days.
      - **Entry Action**: Log appeal receipt and notify Appellate Authority.
      - **Exit Action**: Return to adjudication or finalize resolution.
    - **Key Steps**: 
      - Member submits a written appeal within 30 calendar days of the final determination.
      - The appeal is reviewed by the Appellate Authority (e.g., HQ AFRC/CD).
    - **Timeline**: 30 calendar days for appeal submission.
    - **Authorities**: HQ AFRC/CD (appellate).
    - **Notes**: Runs concurrently with the reinvestigation option, as per 5.7.3, and may lead to re-adjudication.

  - **Sub-Workflow: Reinvestigations**
    - **Description**: Permits the member to request a reinvestigation if new evidence or circumstances warrant further review.
    - **State: ReinvestigationRequested**
      - **Description**: State where the reinvestigation request is under review.
      - **Transition**: ReinvestigationApproved → AuthorityAdjudication | ReinvestigationDenied → Finalized
      - **Trigger**: Adjudicate (if approved, escalates to formal investigation) or Finalize (if denied).
      - **Guard**: Request submitted within 30 calendar days, new evidence evaluated.
      - **Entry Action**: Log request and notify Appointing Authority.
      - **Exit Action**: Escalate to investigation or deny and return to finalized state.
    - **Key Steps**: 
      - Member submits a request for reinvestigation within 30 calendar days of the final determination.
      - The Appointing Authority reviews the request; if warranted, it may escalate to a formal investigation.
    - **Timeline**: 30 calendar days for the request submission, with an additional 90 calendar days if a formal investigation is initiated.
    - **Authorities**: Appointing Authority (initial review), potentially AFRC LOD Determination Board (if escalated).
    - **Notes**: Runs concurrently with the appeal process, per 5.7.3, and aligns with the formal LOD timeline if reinvestigation requires it.

## Summary
- **AFR Informal LOD Determination**: 1 main workflow with 9 states (Initiated, MedicalAssessment, Routing, CommanderReview, LegalReview, AuthorityAdjudication, Finalized, Appealed, Voided) and a 90-day timeline, supported by 2 sub-workflows:
  - **Appeals**: 1 state (Appealed) with a 30-day initial timeline.
  - **Reinvestigations**: 1 state (ReinvestigationRequested) with a 30-day initial timeline, extendable to 90 days if escalated.

This breakdown, created at 10:06 PM EDT on Friday, October 24, 2025, focuses exclusively on the AFR Informal LOD Determination workflow, integrating state machine terminology to align with the Stateless implementation, and includes Appeals and Reinvestigations as sub-workflows.