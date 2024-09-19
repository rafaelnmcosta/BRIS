import React from 'react';
import { Row, Col } from 'antd';
import { useNavigate } from 'react-router-dom';

import '../App.css';
import './Home.css';

import HeadbarSistema from '../components/HeadbarSistema';

const Home = () => {
  const navigate = useNavigate();
  
  // const tipoUsuario = localStorage.getItem('tipoUsuario');
  const tipoUsuario = '2';

  if (!tipoUsuario) {
    return <h1>Você precisa estar logado para acessar esta página.</h1>;
  }

  const renderContent = () => {
    switch (tipoUsuario) {
      case '1': // Admin
        return (
          <>
            <Col span={6}>
              <button className='button-menu' onClick={() => navigate('/avaliacoes')}>Gerenciar avaliações</button>
            </Col>
            <Col span={6}>
              <button className='button-menu' onClick={() => navigate('/animais')}>Gerenciar animais</button>
            </Col>
            <Col span={6}>
              <button className='button-menu' onClick={() => navigate('/usuarios')}>Gerenciar usuários</button>
            </Col>
            <Col span={6}>
              <button className='button-menu' disabled={true} onClick={() => navigate('/granjas')}>Gerenciar granjas</button>
            </Col>
            <Col span={6} style={{marginTop: 20}}>
              <button className='button-menu' disabled={true} onClick={() => navigate('/agroindustrias')}>Gerenciar agroindústrias</button>
            </Col>
          </>
        );

      case '2': // Gestor de Granja
        return (
          <>
            <Col span={8}>
              <button className='button-menu' onClick={() => navigate('/avaliacoes')}>Gerenciar avaliações</button>
            </Col>
            <Col span={8}>
              <button className='button-menu' onClick={() => navigate('/animais')}>Gerenciar animais</button>
            </Col>
            <Col span={8}>
              <button className='button-menu' onClick={() => navigate('/usuarios')}>Gerenciar usuários</button>
            </Col>
          </>
        );

      case '3': // Gestor de Agroindústria
        return (
          <>
            <Col span={8}>
              <button className='button-menu' onClick={() => navigate('/animais')}>Visualizar animais</button>
            </Col>
            <Col span={8}>
              <button className='button-menu' onClick={() => navigate('/usuarios')}>Gerenciar usuários</button>
            </Col>
            <Col span={8}>
              <button className='button-menu button-desativado' disabled={true} onClick={() => navigate('/granjas')}>Gerenciar granjas</button>
            </Col>
          </>
        );

      case '4': // Técnico
        return (
          <>
            <Col span={12}>
              <button className='button-menu' onClick={() => navigate('/avaliacoes')}>Gerenciar avaliações</button>
            </Col>
            <Col span={12}>
              <button className='button-menu' onClick={() => navigate('/animais')}>Gerenciar animais</button>
            </Col>
          </>
        );

      case '5': // Visualizador
        return (
          <>
            <Col span={6}>
              <button className='button-menu' onClick={() => navigate('/avaliacoes')}>Visualizar avaliações</button>
            </Col>
            <Col span={6}>
              <button className='button-menu' onClick={() => navigate('/animais')}>Visualizar animais</button>
            </Col>
            <Col span={6}>
              <button className='button-menu' onClick={() => navigate('/usuarios')}>Visualizar usuários</button>
            </Col>
          </>
        );

      default:
        return <h1>Tipo de usuário inválido.</h1>;
    }
  };

  return (
    <div>
      <HeadbarSistema />
      <div className='page-content'>
        <h2>Bem-vindo ao BRIS!</h2>
        <Row gutter={24} style={{ marginTop: 20}}>
          {renderContent()}
        </Row>
      </div>
    </div>
  );
};

export default Home;
