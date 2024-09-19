import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import Checkbox from './Checkbox';
import '../App.css';
import './FormLogin.css';


import { ReactComponent as IconPessoa } from '../assets/icones/user-svgrepo-com.svg';
import { ReactComponent as IconSenha } from '../assets/icones/lock-svgrepo-com.svg';

const FormLogin = ({ onLogin }) => {
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');
  
    const handleSubmit = (e) => {
      e.preventDefault();
      onLogin(email, senha);
    };
  
    return (
      <form className='form-login' onSubmit={handleSubmit}>
        <h1>LOGIN</h1>
        <div className='lado-a-lado'>
          <IconPessoa className='icone' />
          <label>E-mail</label>
        </div>
        <input
          className='input-form'
          type="text"
          placeholder="Seu e-mail"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <div className='lado-a-lado'>
          <IconSenha className='icone' />
          <label>Senha</label>
        </div>
        <input
          className='input-form'
          type="password"
          placeholder="Sua senha"
          value={senha}
          onChange={(e) => setSenha(e.target.value)}
          required
        />
        <Checkbox texto="Lembrar-me" className="checkbox"/>
        <button className='button-primario' type="submit">ENTRAR</button>
        <Link to='/recurar-senha'>Esqueceu a senha?</Link>
        <br/>
        <p>Ainda não possui uma conta?</p>
        <div className='lado-a-lado'>
          <p>Cadastre-se</p>
          <Link className='aqui' to='/cadastro'>aqui!</Link>
        </div>
      </form>
    );
  };
  
  export default FormLogin;
  