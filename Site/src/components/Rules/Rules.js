import React from 'react';
import './rules.css'; 
import { useTranslation } from 'react-i18next';
function Rules() {
    const { t } = useTranslation();

  return (
    <main>
      <div className="container">
        <section className="rule-section">
          <p>
            <b>
                {t('GameGoal')}
            </b>
          </p>
        </section>
        <section className="rule-section">
          <p>
                      {t('CaptureZone')}
          </p>
          <img
            src="./images/game-strategy.png"
            alt="Game Strategy"
            style={{ maxWidth: "900px" }}
          />
        </section>
        <section className="rule-section">
          <p>
                      <b>{t('GameCore')}</b>
          </p>
        </section>
        <div className="main-grid">
          <section className="rule-section rule-grid-item">
                      <p>
                          {t('Pawn')}
                      </p>
            <img src="./images/rules_pawn.png" alt="Pawn" />
          </section>
		  
		<section class="rule-section rule-grid-item">
                      <p>
                          {t('Knight')}
                      </p>
          <img src="./images/rules_horse.png" alt="Game Strategy" />
        </section>

        <section class="rule-section rule-grid-item">
                      <p>
                          {t('Queen')}
                      </p>
          <img src="./images/rules_queen.png" alt="Game Strategy" />
        </section>

        <section class="rule-section rule-grid-item">
                      <p>
                          {t('King')}
                      </p>
          <img src="./images/rules_king.png" alt="Game Strategy" />
        </section>

        <section class="rule-section rule-grid-item">
                      <p>
                          {t('Castle')}
                      </p>
          <img src="./images/rules_castle.png" alt="Game Strategy" />
        </section>

        <section class="rule-section rule-grid-item">
                      <p>
                          {t('Bishop')}
                      </p>
          <img src="./images/rules_bishop.png" alt="Game Strategy" />
        </section>
		  
		  
		  		  
        </div>
      </div>
    </main>
  );
}

export default Rules;
