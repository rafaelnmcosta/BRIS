import React from 'react';
import { Card } from 'antd';
import './CardListagem.css'

const CardUsuario = ({ id, nome, email, tipo, onEdit }) => {
  return (
    <Card size="small" title={nome} className='card' extra={<a href="/editar" onClick={onEdit}>Editar</a>}>
        <p>E-mail: {email}</p>
        <p>Tipo de usuario: {tipo}</p>
    </Card>
  );
};

export default CardUsuario;
