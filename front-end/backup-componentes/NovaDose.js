import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { Row, Col } from 'antd';

import CardAnimalDose from '../components/CardAnimalDose';
import HeadbarSistema from '../components/HeadbarSistema';

import '../App.css';
import './NovaDose.css';

const NovaDose = () => {
  const [animais, setAnimais] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    axios.get(`http://localhost:5206/api/Animais/`)
      .then(response => {
        setAnimais(response.data);
      })
      .catch(error => {
        console.error('Erro ao buscar os dados dos animais:', error);
      });
  }, []);

  const handleNovaDose = (id) => {
    navigate(`/animais/${id}/nova-dose`);
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
                        <button className='button-nova-dose' onClick={() => handleNovaDose(animal.id)}>
                            Nova Dose
                        </button>
                    </Col>
                    <Col flex='auto'>
                        <CardAnimalDose
                        id={animal.id}
                        info={animal.info}
                        />
                    </Col>
                </Row>
            ))}
        </div>
    </div>
  );
};

export default NovaDose;
