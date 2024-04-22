// src/components/Sidebar.js
import React from 'react';
import { Link } from 'react-router-dom';

const Sidebar = () => {
  return (
    <div style={styles.sidebar}>
      <h2>Menu</h2>
      <ul style={styles.nav}>
        <li>
          <Link to="/home">Home</Link>
        </li>
        <li>
          <Link to="/sobre">Sobre</Link>
        </li>
        <li>
          <Link to="/">Logout</Link>
        </li>
      </ul>
    </div>
  );
};

const styles = {
  sidebar: {
    width: '250px',
    height: '100%',
    backgroundColor: '#333',
    color: '#fff',
    padding: '20px',
    position: 'fixed',
    top: 0,
    left: 0,
  },
  nav: {
    listStyleType: 'none',
    padding: 0,
  },
};

export default Sidebar;
