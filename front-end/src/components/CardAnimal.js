import React from 'react';
import { Card } from 'antd';
import './CardAnimal.css'

const CardAnimal = ({ id, info, onEdit }) => {
  return (
    <Card size="small" title={'Animal de id: ' + id} className='card' extra={<a href="/editar" onClick={onEdit}>Editar</a>}>
      <p>Nome: {info}</p>
    </Card>
  );
};

export default CardAnimal;
