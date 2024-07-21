import React from 'react';
import { Card } from 'antd';
import { Link, useNavigate } from 'react-router-dom';
import './CardListagem.css'

const CardAnimal = ({ id, info }) => {
  const navigate = useNavigate();

  const handleAcessar = (id) => {
    navigate(`/animais/${id}`);
  };
  
  return (
    <Card size="small" title={<Link to={`/animais/${id}`}>Animal de id: {id}</Link>} className='card' extra={<Link to={`/animais/${id}/editar`}>Editar</Link>}>
      <p>{info}</p>
      <button className='button-secundario' onClick={() => handleAcessar(id)}>Acessar</button>
    </Card>
  );
};

export default CardAnimal;
