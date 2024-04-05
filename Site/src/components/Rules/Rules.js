import React from 'react';
import './rules.css'; 

function Rules() {
  return (
    <main>
      <div className="container">
        <section className="rule-section">
          <p>
            <b>
              The goal of the game is to capture as many cells as possible. The
              game ends when there are no white cells left, and the player who
              captured the most cells wins.
            </b>
          </p>
        </section>
        <section className="rule-section">
          <p>
            If a player has captured a 3x3 cell area, the enemy will no longer be
            able to walk on them, and they will become a little darker.
          </p>
          <img
            src="./images/game-strategy.png"
            alt="Game Strategy"
            style={{ maxWidth: "900px" }}
          />
        </section>
        <section className="rule-section">
          <p>
            <b>The heart of the game is the figures and their movement patterns.</b>
          </p>
        </section>
        <div className="main-grid">
          <section className="rule-section rule-grid-item">
            <p>The pawn is the only one combat piece. It can eat pieces within one square of it. Can capture enemy squares.</p>
            <img src="./images/rules_pawn.png" alt="Pawn" />
          </section>
		  
		<section class="rule-section rule-grid-item">
          <p>
            The horse looks like a horse from chess. He walks the same way and
            captures all the cells on his way. Can capture enemy squares.
          </p>
          <img src="./images/rules_horse.png" alt="Game Strategy" />
        </section>

        <section class="rule-section rule-grid-item">
          <p>
            The queen moves like in chess, but cannot capture enemy squares.
          </p>
          <img src="./images/rules_queen.png" alt="Game Strategy" />
        </section>

        <section class="rule-section rule-grid-item">
          <p>The king can move to any square except the enemy one.</p>
          <img src="./images/rules_king.png" alt="Game Strategy" />
        </section>

        <section class="rule-section rule-grid-item">
          <p>
            The castle moves vertically and horizontally. Can capture enemy
            squares.
          </p>
          <img src="./images/rules_castle.png" alt="Game Strategy" />
        </section>

        <section class="rule-section rule-grid-item">
          <p>
            The bishop moves the same way as in chess. Can capture enemy
            squares.
          </p>
          <img src="./images/rules_bishop.png" alt="Game Strategy" />
        </section>
		  
		  
		  		  
        </div>
      </div>
    </main>
  );
}

export default Rules;
