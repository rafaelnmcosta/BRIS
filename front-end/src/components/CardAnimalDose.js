import React from 'react';
import { Card } from 'antd';
import './CardListagem.css'

const CardAnimalDose = ({ id, info }) => {
  return (
    <Card size="small" title={'Animal de id: ' + id} className='card'>
      <p>Nome: {info}</p>
    </Card>
  );
};

export default CardAnimalDose;
