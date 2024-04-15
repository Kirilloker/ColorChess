import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import i18n from './i18n';
import Main from './components/Main/Main';
import Rules from './components/Rules/Rules';
import Top from './components/Top/Top';
import Download from './components/Download/Download';
import Footer from './components/Footer/Footer'; 
import './App.css';
import { useTranslation } from 'react-i18next';

function App() {
    const { t, i18n } = useTranslation();

    const changeLanguage = (language) => {
        if (i18n.isInitialized) {
            console.error('i18n initialized');
            i18n.changeLanguage(language);
        } else {
            console.error('i18n not initialized');
        }
    };

    return (
        <Router>
            <div className="app-container">
                <header>
                    <nav>
                        <ul>
                            <li><a href="/">Color Chess</a></li>
                            <li>|</li>
                            <li><Link to="/">{t('Main')}</Link></li>
                            <li><Link to="/rules">{t('Rules')}</Link></li>
                            <li><Link to="/top">{t('Top')}</Link></li>
                            <li><Link to="/download">{t('Download')}</Link></li>
                            <li className="lang-switch">
                                <span onClick={() => changeLanguage('ru')}>RU</span>| 
                                <span onClick={() => changeLanguage('en')}>EN</span>
                            </li>
                        </ul>
                    </nav>
                </header>
                <Routes>
                    <Route path="/" element={<Main />} />
                    <Route path="/rules" element={<Rules />} />
                    <Route path="/top" element={<Top />} />
                    <Route path="/download" element={<Download />} />
                </Routes>
                <Footer />
            </div>
        </Router>
    );
}

export default App;
