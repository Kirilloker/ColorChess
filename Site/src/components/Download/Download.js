import React from 'react';
import './download.css';

function Download() {
    return (
        <main>
            <section className="version-section">
                <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                <h2>Version 1.0.31</h2>
                <div className="platform-icons">
                    <a href="/Files/Build.zip" download>
                        <img src="./images/windows-icon.png" alt="Windows" />
                    </a>
                    <a href="/Files/Build.zip" download>
                        <img src="./images/apple-icon.png" alt="Apple" />
                    </a>
                    <a href="/Files/Build.zip" download>
                        <img src="./images/android-icon.png" alt="Android" />
                    </a>
                </div>
            </section>
        </main>
    );
}

export default Download;
