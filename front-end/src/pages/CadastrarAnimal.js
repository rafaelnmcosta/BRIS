import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import HeadbarSistema from '../components/HeadbarSistema';

import { ReactComponent as IconInfo } from '../assets/icones/information-letter-outline-svgrepo-com.svg';

import './Cadastrar.css';

const CadastrarAnimal = () => {
    const [animal, setAnimal] = useState('');
    const [erro, setErro] = useState('');
    const navigate = useNavigate();
    const token = localStorage.getItem('jwtToken');

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await api.post('http://localhost:5206/api/Animais', animal, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            alert('Cadastro realizado com sucesso!');
            navigate('/animais');
            return response;
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
                        <IconInfo className='icone' />
                        <label>Linhagem</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="text"
                        placeholder="Informações sobre a linhagem do novo animal a ser cadastrado"
                        value={animal.linhagem}
                        onChange={(e) => setAnimal(e.target.value)}
                        required
                    />
                    <div className='lado-a-lado'>
                        <IconInfo className='icone' />
                        <label>Idade</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="number"
                        placeholder="Idade do novo animal a ser cadastrado"
                        value={animal.idade}
                        onChange={(e) => setAnimal(e.target.value)}
                        required
                    />
                    <div className='lado-a-lado'>
                        <IconInfo className='icone' />
                        <label>Peso</label>
                    </div>
                    <input
                        className='input-cadastrar'
                        type="number"
                        placeholder="Peso do novo animal a ser cadastrado"
                        value={animal.peso}
                        onChange={(e) => setAnimal(e.target.value)}
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

export default CadastrarAnimal;
