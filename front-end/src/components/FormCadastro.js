import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';  // Importa o axios para fazer requisições HTTP
import '../App.css';
import './FormCadastro.css';

import { ReactComponent as IconUsuario } from '../assets/icones/user-svgrepo-com.svg';
import { ReactComponent as IconSenha } from '../assets/icones/lock-svgrepo-com.svg';
import { ReactComponent as IconEmail } from '../assets/icones/mail-svgrepo-com.svg';

const FormCadastro = () => {
    const [nome, setNome] = useState('');
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');
    const [confirmarSenha, setConfirmarSenha] = useState('');  // Adiciona um estado para confirmar a senha
    const [erro, setErro] = useState('');  // Adiciona um estado para exibir mensagens de erro

    const handleSubmit = async (e) => {
      e.preventDefault();
      
      if (senha !== confirmarSenha) {
        setErro('As senhas não coincidem.');
        return;
      }

      try {
        const response = await axios.post('http://localhost:5206/api/Usuarios/cadastro', {
          nome,
          email,
          senha
        });
        alert('Cadastro realizado com sucesso!');
        // Redirecionar ou limpar o formulário, se necessário
      } catch (error) {
        console.error('Erro ao cadastrar usuário:', error);
        setErro('Erro ao realizar o cadastro. Tente novamente.');
      }
    };

    return (
      <form className='form-cadastro' onSubmit={handleSubmit}>
        <h2 className='titulo-cadastro'>CADASTRO</h2>
        <p className='texto-cadastro'>Já possui cadastro?</p>
        <div className='lado-a-lado'>
          <p>Pode entrar no sistema clicando</p>
          <Link className='aqui' to='/login'>aqui!</Link>
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
          <IconEmail className='icone' />
          <label>E-mail</label>
        </div>
        <input
          className='input-form'
          type="email"
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
          value={confirmarSenha}
          onChange={(e) => setConfirmarSenha(e.target.value)}
          required
        />
        {erro && <p className='erro'>{erro}</p>}  {/* Exibe mensagem de erro, se houver */}
        <br/>
        <button className='button-primario' type="submit">CADASTRAR</button>
      </form>
    );
};

export default FormCadastro;
