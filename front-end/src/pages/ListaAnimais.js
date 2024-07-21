import React, { useState, useEffect } from 'react';
import axios from 'axios';
import CardAnimal from '../components/CardAnimal';
import HeadbarSistema from '../components/HeadbarSistema';

import './Listas.css';

import { ReactComponent as IconCadastro } from '../assets/icones/plus-add-cross-outline-svgrepo-com.svg';

const ListaAnimais = () => {
  const [animais, setAnimais] = useState([]);

  useEffect(() => {
    const fetchAnimais = async () => {
      try {
        const response = await axios.get('http://localhost:5206/api/Animais');
        setAnimais(response.data);
      } catch (error) {
        console.error('Erro ao buscar os dados dos animais:', error);
      }
    };

    fetchAnimais();
  }, []);

  const handleEdit = (animalId) => {
    // Lógica para editar os dados do animal com o ID fornecido
    console.log('Editar dados do animal com ID:', animalId);
  };

  return (
    <div>
        <HeadbarSistema />
        <div className='page-content'>
            <a href='/'> {'< '} Voltar</a>
            <div className='lado-a-lado'>
              <h2 className='title'>Lista de animais</h2>
              <div className='button-bar'>
                <button className='button-com-icone'>
                  <IconCadastro className='icone-botao'/>
                  Cadastrar novo animal
                </button>
              </div>
            </div>
            {animais.map(animal => (
                <div key={animal.id}>
                    <CardAnimal
                    id={animal.id}
                    info={animal.info}
                    onEdit={() => handleEdit(animal.id)}
                    />
                </div>
            ))}
        </div>
    </div>
  );
};

export default ListaAnimais;
