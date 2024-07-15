import React, { useState, useEffect } from 'react';
import axios from 'axios';
import CardAnimal from '../components/CardAnimal';
import HeadbarSistema from '../components/HeadbarSistema';

const ListaAnimais = () => {
  const [animais, setAnimais] = useState([]);

  const AnimaisTeste = [
    { id: 1, info: 'Bobby'},
    { id: 2, info: 'Luna'},
    { id: 3, info: 'Max'},
    { id: 4, info: 'Bombom'},
  ];

  useEffect(() => {
    /*
    axios.get('https://api.exemplo.com/animais')
      .then(response => {
        setAnimais(response.data);
      })
      .catch(error => {
        console.error('Erro ao buscar os dados dos animais:', error);
      });
    */
   setAnimais(AnimaisTeste);
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
            <h2>Lista de animais</h2>
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
