import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import CardAnimal from '../components/CardAnimal';
import HeadbarSistema from '../components/HeadbarSistema';

import './Listas.css';

import { ReactComponent as IconCadastro } from '../assets/icones/plus-add-cross-outline-svgrepo-com.svg';

const ListaAnimais = () => {
  const [animais, setAnimais] = useState([]);
  const navigate = useNavigate();

  // Obtém o tipo de usuário do localStorage
  const tipoUsuario = localStorage.getItem('tipoUsuario');
  // const tipoUsuario = '1';

  useEffect(() => {
    const fetchAnimais = async () => {
      try {
        const response = await api.get('http://localhost:5206/api/Animais');
        setAnimais(response.data);
      } catch (error) {
        console.error('Erro ao buscar os dados dos animais:', error);
      }
    };

    fetchAnimais();
  }, []);

  const handleEdit = (animalId) => {
    console.log('Editar dados do animal com ID:', animalId);
    // Lógica de edição
  };

  return (
    <div>
      <HeadbarSistema />
      <div className='page-content'>
        <a href='/'> {'< '} Voltar</a>
        <div className='lado-a-lado'>
          <h2 className='title'>Lista de animais</h2>
          {tipoUsuario === '1' || tipoUsuario === '2' || tipoUsuario === '4' ? (
            <div className='button-bar'>
              <button onClick={() => navigate('/animais/cadastrar')} className='button-com-icone'>
                <IconCadastro className='icone-botao' />
                Cadastrar novo animal
              </button>
            </div>
          ) : null}
        </div>
        {animais.map(animal => (
          <div key={animal.id}>
            <CardAnimal
              id={animal.id}
              info={animal.info}
              onEdit={tipoUsuario === '1' || tipoUsuario === '2' || tipoUsuario === '4' ? () => handleEdit(animal.id) : null}
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default ListaAnimais;
