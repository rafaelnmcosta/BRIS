import React from 'react';
import { Card } from 'antd';
import { Link } from 'react-router-dom';
import './CardListagem.css';

const CardUsuario = ({ id, nome, email, tipo }) => {
  return (
    <Card size="small" title={<Link to={`/usuarios/${id}`}><h3>{nome}</h3></Link>} className='card' extra={<Link to={`/usuarios/${id}/editar`}>Editar</Link>}>
        <p>E-mail: {email}</p>
        <p>Tipo de usuario: {tipo}</p>
    </Card>
  );
};

export default CardUsuario;
