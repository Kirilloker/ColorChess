import React from 'react';
import './main.css';  

function Main() {
  return (
    <main>
      <section className="full-width-image-section">
        <img src="./images/chess-desk-setup.png" alt="Desktop Setup" className="full-width-image" />
      </section>
      <div className="container">
        <section className="content-section">
          <img src="./images/chess-piece.png" alt="Chess Piece" className="half-width-image" />
          <div className="text-content" style={{ fontSize: "35px" }}>
            <p>
              Color Chess is a multiplayer game where the goal is to capture and paint as many cells as possible with
              your unique moving pieces.
            </p>
          </div>
        </section>
        <section className="content-section">
          <div className="text-content" style={{ fontSize: "35px" }}>
            <p>
              Color Chess is a strategic game that enhances decision-making and allows players to develop their own
              unique playing style.
            </p>
          </div>
          <img src="./images/chess-board.png" alt="Chess Board" className="half-width-image2" />
        </section>
      </div>
    </main>
  );
}

export default Main;
