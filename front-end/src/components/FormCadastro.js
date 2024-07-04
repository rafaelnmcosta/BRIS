import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import '../App.css';
import './FormCadastro.css';


import { ReactComponent as IconUsuario } from '../assets/icones/user-svgrepo-com.svg';
import { ReactComponent as IconSenha } from '../assets/icones/lock-svgrepo-com.svg';
import { ReactComponent as IconEmail } from '../assets/icones/mail-svgrepo-com.svg';


const FormCadastro = () => {
    const [nome, setNome] = useState('');
    const [usuario, setUsuario] = useState('');
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');

    const handleSubmit = (e) => {
      e.preventDefault();
    };
  
    return (
      <form className='form-cadastro' onSubmit={handleSubmit}>
        <h2 className='titulo-cadastro'>CADASTRO</h2>
        <p className='texto-cadastro'>Já possui cadastro?</p>
        <div className='lado-a-lado'>
          <p>Pode entrar no sistema clicando</p>
          <Link className='aqui' to='/'>aqui!</Link>
        </div>
        <br/>
        <div className='lado-a-lado'>
          <IconUsuario className='icone' />
          <label>Nome</label>
        </div>
        <input
          className='input-form'
          type="text"
          placeholder="Seu nome completo"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          required
        />
        <div className='lado-a-lado'>
          <IconUsuario className='icone' />
          <label>Nome de usuario</label>
        </div>
        <input
          className='input-form'
          type="text"
          placeholder="Seu nome de usuário no sistema"
          value={usuario}
          onChange={(e) => setUsuario(e.target.value)}
          required
        />
        <div className='lado-a-lado'>
          <IconEmail className='icone' />
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
          placeholder="Insira sua senha"
          value={senha}
          onChange={(e) => setSenha(e.target.value)}
          required
        />
        <div className='lado-a-lado'>
          <IconSenha className='icone' />
          <label>Confirme sua senha</label>
        </div>
        <input
          className='input-form'
          type="password"
          placeholder="Insira sua senha novamente"
          value={senha}
          onChange={(e) => setSenha(e.target.value)}
          required
        />
        <br/>
        <button className='button-primario' type="submit">CADASTRAR</button>
      </form>
    );
  };
  
  export default FormCadastro;
  