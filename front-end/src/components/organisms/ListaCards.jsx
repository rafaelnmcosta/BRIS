import React from 'react';
import CardInfo from '../molecules/CardInfo';

const ListaCards = ({ tipo, lista }) => {
  return (
    <div className="grid grid-cols-1">
      {lista.map((item) => (
        <CardInfo  key={item.id} item={item} tipo={tipo}/>
      ))}
    </div>
  );
};

export default ListaCards;
