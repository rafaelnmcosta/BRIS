// src/components/LoginForm.js
import React from 'react';

const Login = () => {
  return (
    <div style={styles.container}>
      <div style={styles.card}>
        <h2>Login</h2>
        <form>
          <div style={styles.inputGroup}>
            <label htmlFor="usuario">usuario:</label>
            <input type="text" id="usuario" name="usuario" />
          </div>
          <div style={styles.inputGroup}>
            <label htmlFor="senha">Senha:</label>
            <input type="password" id="senha" name="senha" />
          </div>
          <div style={styles.buttonGroup}>
            <button type="submit">Login</button>
            <a href="/recuperar-senha">Esqueceu a senha?</a>
          </div>
        </form>
      </div>
    </div>
  );
};

const styles = {
  container: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '100vh',
  },
  card: {
    padding: '20px',
    border: '1px solid #ccc',
    borderRadius: '8px',
    boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
    width: '300px',
    textAlign: 'center',
  },
  inputGroup: {
    marginBottom: '10px',
    textAlign: 'left',
  },
  buttonGroup: {
    marginTop: '20px',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
};

export default Login;
