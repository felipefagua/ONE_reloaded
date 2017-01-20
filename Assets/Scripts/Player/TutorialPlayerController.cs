using UnityEngine;
using System.Collections;
using System;

public class TutorialPlayerController : PlayerController {

    public System.Action<EAfinityType> OnMoonDetectedOnTutorial;

    private TutorialMenuController _tutorialMenu;

    protected override void InitExternalComponents() {
        base.InitExternalComponents();
        _tutorialMenu = FindObjectOfType<TutorialMenuController>();
    }

    protected override void OnMoonDetected(EAfinityType type, float afinity, int count) {
        base.OnMoonDetected(type, afinity, count);
        OnMoonDetectedOnTutorial(type);
    }

    public override void SetPulseAndLife(float afinity) {
        if (afinity < 0.6f && !InBonusMode && CanDecrementLives()) {
            if (CurrentLife >= 0) {
                CurrentLife--;
                if (CurrentLife <= 0)
                    CurrentLife = 1;
                OnUpdateLives();
            }
            
            m_Pulse.Speed += m_PulseStep;

            if (CurrentLife < Lifes) {
                m_Damage.DamageIndex = CurrentLife - 1;
                m_Pulse.StateIndex = CurrentLife - 1;
                m_Damage.Speed += m_PulseStep;
            }
        }
    }

    public override void CalculatePerformance(EAfinityType type, float afinity, int count) {
        if (CanIncrementScore())
            IncrementScore(type, count);

        if (CanIncrementXP())
            IncrementXP(type, afinity, count);
    }

    private bool CanDecrementLives() {
        return (_tutorialMenu.CurrentTutorialStep >= 
            TutorialMenuController.TutorialStep.lives_intro);
    }

    private bool CanIncrementScore() {
        return (_tutorialMenu.CurrentTutorialStep >= 
            TutorialMenuController.TutorialStep.xp_counter);
    }

    private bool CanIncrementXP() {
        return (_tutorialMenu.CurrentTutorialStep >= 
            TutorialMenuController.TutorialStep.lives_counter);
    }
}
