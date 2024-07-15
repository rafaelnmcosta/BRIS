import React from 'react';
import { Row, Col, Button } from 'antd';
import { useNavigate } from 'react-router-dom';

import '../App.css';
import './Home.css';

import HeadbarSistema from '../components/HeadbarSistema';

const Home = () => {
  const navigate = useNavigate();

  return (
    <div>
      <HeadbarSistema />
      <div className='page-content'> 
        <h2>Bem-vindo ao BRIS!</h2>
        <Row gutter={24} style={{ marginTop: 20 }}>
          <Col span={6}>
            <button className='button-menu' onClick={()=> navigate('/nova-dose')}>Cadastrar nova dose</button>
          </Col>
          <Col span={6}>
            <button className='button-menu' onClick={()=> navigate('/animais')}>Gerenciar animais</button>
          </Col>
          <Col span={6}>
            <button className='button-menu' onClick={()=> navigate('/usuarios')}>Gerenciar usuários</button>
          </Col>
          <Col span={6}>
            <button className='button-menu' onClick={()=> navigate('/granjas')}>Gerenciar granjas</button>
          </Col>
        </Row>
      </div>
    </div>
  );
};

export default Home;
