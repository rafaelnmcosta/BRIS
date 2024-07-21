import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import HeadbarSistema from '../components/HeadbarSistema';

import { ReactComponent as IconInfo } from '../assets/icones/information-letter-outline-svgrepo-com.svg';
import { ReactComponent as IconId } from '../assets/icones/notebook-with-text-lines-outline-svgrepo-com.svg';

import './Cadastrar.css';

const CadastrarUsuario = () => {
    const [id, setId] = useState('');
    const [info, setInfo] = useState('');
    const [erro, setErro] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await api.post('http://localhost:5206/api/Animais', {
                id,
                info,
            });
            alert('Cadastro realizado com sucesso!');
            navigate('/animais');
        } catch (error) {
            console.error('Erro ao cadastrar animal:', error);
            setErro('Erro ao realizar o cadastro. Tente novamente.');
        }
    };

    return (
        <div>
            <HeadbarSistema />
            <div className='page-content'>
                <a href='/animais'> {'< '} Voltar</a>
                <h2 className='titulo-cadastro'>Cadastro de novo animal</h2>
                <form onSubmit={handleSubmit} className='form-cadastrar'>
                    <div className='lado-a-lado'>
                        <IconId className='icone' />
                        <label>Id:</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="text"
                        placeholder="Numero de identificação do animal"
                        value={id}
                        onChange={(e) => setId(e.target.value)}
                        required
                    />
                    <div className='lado-a-lado'>
                        <IconInfo className='icone' />
                        <label>Informações</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="text"
                        placeholder="Informações sobre o novo animal a ser cadastrado"
                        value={info}
                        onChange={(e) => setInfo(e.target.value)}
                        required
                    />
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
