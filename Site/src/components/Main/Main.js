import React from 'react';
import './main.css';  
import { useTranslation } from 'react-i18next';

function Main() {
    const { t } = useTranslation();

    return (
        <main>
            <section className="full-width-image-section">
                <img src="./images/chess-desk-setup.png" alt="Desktop Setup" className="full-width-image" />
            </section>

            <section className="content-section">

            <div className="text-content">
                <p>
                    {t('MultiplayerGame')}
                </p>
            </div>

            <img src="./images/chess-piece.png" alt="Chess Piece" className="half-width-image" />

            </section>

            <section className="content-section">
            <div className="text-content">
                    <p>
                        {t('StrategyGame')}
                    </p>
                </div>

                <img src="./images/chess-board.png" alt="Chess Board" className="half-width-image2" />
            </section>
        </main>
    );
}


export default Main;
