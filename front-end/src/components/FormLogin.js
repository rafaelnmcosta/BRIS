import React, { useState } from 'react';
import ReCAPTCHA from "react-google-recaptcha";
import Checkbox from './Checkbox';
import '../App.css';
import './FormLogin.css';


import { ReactComponent as IconPessoa } from '../assets/icones/user-svgrepo-com.svg';
import { ReactComponent as IconSenha } from '../assets/icones/lock-svgrepo-com.svg';

const Form_login = ({ onLogin, onShowForgotPasswordForm }) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isCaptchaVerified, setIsCaptchaVerified] = useState(false);
  
    const handleCaptchaChange = (value) => {
      setIsCaptchaVerified(true);
    };
  
    const handleSubmit = (e) => {
      e.preventDefault();
      /*if (!isCaptchaVerified) {
        alert('Por favor, verifique o Captcha');
        return;
      }*/
      onLogin(email, password);
    };
  
    return (
      <form className='form-login' onSubmit={handleSubmit}>
        <h1>LOGIN</h1>
        <div className='lado-a-lado'>
          <IconPessoa className='icone' />
          <label>Usuário</label>
        </div>
        <input
          className='input-form'
          type="email"
          placeholder="Seu nome de usuário"
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
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <Checkbox texto="Lembrar-me" className="checkbox"/>
        <button className='button-primario' type="submit">ENTRAR</button>
        <a>Esqueceu a senha?</a>
        <br/>
        <p>Ainda não possui uma conta?</p>
        <div className='lado-a-lado'>
          <p>Cadastre-se </p>
          <a className='aqui'>aqui!</a>
          </div>
      </form>
    );
  };
  
  export default Form_login;
  