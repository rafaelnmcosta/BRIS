import React, { useState } from 'react';
import axios from 'axios';


import { ReactComponent as IconUsuario } from '../assets/icones/user-svgrepo-com.svg';
import { ReactComponent as IconSenha } from '../assets/icones/lock-svgrepo-com.svg';
import { ReactComponent as IconEmail } from '../assets/icones/mail-svgrepo-com.svg';

const CadastrarUsuario = () => {
    const [nome, setNome] = useState('');
    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');
    const [confirmarSenha, setConfirmarSenha] = useState('');
    const [tipoUsuario, setTipoUsuario] = useState(0);  // Adiciona estado para tipo de usuário
    const [erro, setErro] = useState('');

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
                senha,
                tipoUsuario  // Adiciona o tipo de usuário ao corpo da requisição
            });
            alert('Cadastro realizado com sucesso!');
            setNome('');
            setEmail('');
            setSenha('');
            setConfirmarSenha('');
            setTipoUsuario(0);  // Reseta o tipo de usuário para o valor padrão
        } catch (error) {
            console.error('Erro ao cadastrar usuário:', error);
            setErro('Erro ao realizar o cadastro. Tente novamente.');
        }
    };

    return (
        <form className='form-cadastro' onSubmit={handleSubmit}>
            <h2 className='titulo-cadastro'>CADASTRO</h2>
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
            <div className='lado-a-lado'>
                <label>Tipo de Usuário</label>
            </div>
            <select
                className='input-form'
                value={tipoUsuario}
                onChange={(e) => setTipoUsuario(Number(e.target.value))}
                required
            >
                <option value={0}>Admin</option>
                <option value={1}>Gerente</option>
                <option value={3}>Técnico</option>
            </select>
            {erro && <p className='erro'>{erro}</p>}
            <br/>
            <button className='button-primario' type="submit">CADASTRAR</button>
        </form>
    );
};

export default CadastrarUsuario;
