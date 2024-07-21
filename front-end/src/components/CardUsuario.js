import React from 'react';
import { Card } from 'antd';
import { Link, useNavigate } from 'react-router-dom';
import './CardListagem.css';

const CardUsuario = ({ id, nome, email, tipo }) => {
  const navigate = useNavigate();

  const handleAcessar = () => {
    navigate(`/usuarios/${id}`);
  };
  return (
    <Card size="small" title={<Link to={`/usuarios/${id}`}><h3>{nome}</h3></Link>} className='card' extra={<Link to={`/usuarios/${id}/editar`}>Editar</Link>}>
        <p>E-mail: {email}</p>
        <p>Tipo de usuario: {tipo}</p>
        <button className='button-secundario' onClick={handleAcessar}>Acessar</button>
    </Card>
  );
};

export default CardUsuario;
