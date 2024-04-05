import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Main from './components/Main/Main';
import Rules from './components/Rules/Rules';
import Top from './components/Top/Top';
import Download from './components/Download/Download';
import Footer from './components/Footer/Footer'; 
import './App.css';

function App() {
  return (
    <Router>
      <div className="app-container"> 
        <header>
          <nav>
            <ul>
			  <li><a href="/">Color Chess</a></li>
				<li>|</li>
              <li><Link to="/">Main</Link></li>
              <li><Link to="/rules">Rules</Link></li>
              <li><Link to="/top">Top</Link></li>
              <li><Link to="/download">Download</Link></li>
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
