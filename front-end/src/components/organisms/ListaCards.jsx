import React from 'react';
import CardInfo from '../molecules/CardInfo';

const ListaCards = ({ tipoEntidade, lista }) => {
  return (
    <div className="grid grid-cols-1">
      {lista.map((item) => (
        <CardInfo  key={item.id} entidade={item} tipoEntidade={tipoEntidade}/>
      ))}
    </div>
  );
};

export default ListaCards;
