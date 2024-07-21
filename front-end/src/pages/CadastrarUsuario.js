import React, { useState } from 'react';
import api from '../services/api';
import HeadbarSistema from '../components/HeadbarSistema';

import { ReactComponent as IconUsuario } from '../assets/icones/user-svgrepo-com.svg';
import { ReactComponent as IconSenha } from '../assets/icones/lock-svgrepo-com.svg';
import { ReactComponent as IconEmail } from '../assets/icones/mail-svgrepo-com.svg';
import { ReactComponent as IconRoles } from '../assets/icones/users-button-outline-svgrepo-com.svg';

import './Cadastrar.css';

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
            const response = await api.post('http://localhost:5206/api/Usuarios/usuarios/cadastrar', {
                nome,
                email,
                tipoUsuario,
                senha
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
        <div>
            <HeadbarSistema />
            <div className='page-content'>
                <a href='/usuarios'> {'< '} Voltar</a>
                <h2 className='titulo-cadastro'>Cadastro de novo usuário</h2>
                <form onSubmit={handleSubmit} className='form-cadastrar'>
                    <div className='lado-a-lado'>
                        <IconUsuario className='icone' />
                        <label>Nome</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="text"
                        placeholder="Nome completo do novo usuário"
                        value={nome}
                        onChange={(e) => setNome(e.target.value)}
                        required
                    />
                    <div className='lado-a-lado'>
                        <IconEmail className='icone' />
                        <label>E-mail</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="email"
                        placeholder="E-mail do novo usuário"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                    <div className='lado-a-lado'>
                        <IconSenha className='icone' />
                        <label>Senha</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="password"
                        placeholder="Insira a senha"
                        value={senha}
                        onChange={(e) => setSenha(e.target.value)}
                        required
                    />
                    <div className='lado-a-lado'>
                        <IconSenha className='icone' />
                        <label>Confirme a senha</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="password"
                        placeholder="Insira a senha novamente"
                        value={confirmarSenha}
                        onChange={(e) => setConfirmarSenha(e.target.value)}
                        required
                    />
                    <div className='lado-a-lado'>
                        <IconRoles className='icone' />
                        <label>Tipo de Usuário</label>
                    </div>
                    <select
                        className='select-form'
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
                    <div className='lado-a-lado'>
                        <button className='button-primario' type="submit">CADASTRAR</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default CadastrarUsuario;
