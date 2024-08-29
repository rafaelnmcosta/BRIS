import React from 'react';
import { Row, Col } from 'antd';
import { useNavigate } from 'react-router-dom';

import '../App.css';
import './Home.css';

import HeadbarSistema from '../components/HeadbarSistema';

const HomeVisualizador = () => {
  const navigate = useNavigate();

  return (
    <div>
      <HeadbarSistema />
      <div className='page-content'> 
        <h2>Bem-vindo ao BRIS!</h2>
        <Row gutter={24} style={{ marginTop: 20 }}>
          <Col span={6}>
            <button className='button-menu' onClick={()=> navigate('/lista-avaliacoes')}>Visualizar avaliações</button>
          </Col>
          <Col span={6}>
            <button className='button-menu' onClick={()=> navigate('/lista-animais')}>Visualizar animais</button>
          </Col>
          <Col span={6}>
            <button className='button-menu' onClick={()=> navigate('/lista-usuarios')}>Visualizar usuários</button>
          </Col>
        </Row>
      </div>
    </div>
  );
};

export default HomeVisualizador;
