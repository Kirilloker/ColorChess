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
    const [menuActive, setMenuActive] = React.useState(false);

    const changeLanguage = (language) => {
        i18n.changeLanguage(language);
    };

    const toggleMenu = () => {
        setMenuActive(!menuActive);
    };

    // Добавим обработчик для закрытия меню при клике вне его
    const closeMenu = (e) => {
        if (menuActive && !e.target.closest('nav')) {
            setMenuActive(false);
        }
    };

    return (
        <div className="app-container" onClick={closeMenu}>
            <Router>
                <header>
                    <div className="header-title">Color Chess</div>
                    <div className="hamburger-menu" onClick={toggleMenu}>☰</div>
                    <nav>
                        <ul className={menuActive ? "active" : ""}>
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
            </Router>
        </div>
    );
}


export default App;
