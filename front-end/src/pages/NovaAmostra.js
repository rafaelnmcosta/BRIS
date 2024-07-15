import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Row, Col } from 'antd';

import CardAnimal from '../components/CardAnimal';
import HeadbarSistema from '../components/HeadbarSistema';

import '../App.css';
import './NovaAmostra.css';

const NovaAmostra = () => {
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

  const handleNovaAmostra = (animalId) => {
    // Lógica para cadastrar nova amostra para o animal selecionado
    console.log('Nova amostra para o animal com ID:', animalId);
  };

  return (
    <div>
        <HeadbarSistema />
        <div className='page-content'>
            <a href='/'> {'< '} Voltar</a>
            <h2>Selecione o animal que receberá uma nova amostra:</h2>
            {animais.map(animal => (
                <Row key={animal.id}>
                    <Col flex='none'>
                        <button className='button-nova-dose' onClick={() => handleNovaAmostra(animal.id)}>
                            Nova Dose
                        </button>
                    </Col>
                    <Col flex='auto'>
                        <CardAnimal
                        id={animal.id}
                        info={animal.info}
                        onEdit={() => handleEdit(animal.id)}
                        />
                    </Col>
                </Row>
            ))}
        </div>
    </div>
  );
};

export default NovaAmostra;
