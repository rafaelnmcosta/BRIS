// src/components/Home.js
import React from 'react';
import Sidebar from '../components/sidebar';

const Home = () => {
  return (
    <div>
      <Sidebar />
      <div style={styles.content}>
        <h1>Bem-vindo à Página Inicial</h1>
      </div>
    </div>
  );
};

const styles = {
  content: {
    padding: '20px',
  },
};

export default Home;
