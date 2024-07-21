import React, { useState } from 'react';
import { Card, Modal, Radio } from 'antd';
import { Link, useNavigate } from 'react-router-dom';
import api from '../services/api';
import './CardListagem.css';

const CardUsuarioPendente = ({ id, nome, email }) => {
  const [visible, setVisible] = useState(false);
  const [tipoUsuario, setTipoUsuario] = useState(null);
  const navigate = useNavigate();

  const showModal = () => {
    setVisible(true);
  };

  const handleOk = async () => {
    if (tipoUsuario !== null) {
      try {
        console.log(tipoUsuario);
        await api.post(`http://localhost:5206/api/Usuarios/usuarios/ativar/${id}`, { tipoUsuario: tipoUsuario });
        alert('Usuário ativado com sucesso!');
        setVisible(false);
        navigate('/usuarios');
      } catch (error) {
        console.error('Erro ao ativar usuário:', error);
        alert('Erro ao ativar usuário. Tente novamente.');
      }
    } else {
      alert('Por favor, selecione um tipo de usuário.');
    }
  };

  const handleCancel = () => {
    setVisible(false);
  };

  return (
    <>
      <Card size="small" title={<Link to={`/usuarios/${id}`}><h3>{nome}</h3></Link>} className='card'>
        <p>E-mail: {email}</p>
        <button className='button-secundario' onClick={showModal}>Ativar</button>
      </Card>
      <Modal
        title="Selecionar Tipo de Usuário"
        visible={visible}
        onOk={handleOk}
        onCancel={handleCancel}
        okText="Confirmar"
        cancelText="Cancelar"
      >
        <Radio.Group onChange={(e) => setTipoUsuario(e.target.value)} value={tipoUsuario}>
          <Radio value={1}>Admin</Radio>
          <Radio value={2}>Gerente</Radio>
          <Radio value={3}>Técnico</Radio>
        </Radio.Group>
      </Modal>
    </>
  );
};

export default CardUsuarioPendente;
