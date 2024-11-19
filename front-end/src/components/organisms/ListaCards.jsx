import React from 'react';
import CardInfo from '../molecules/CardInfo';

const ListaCards = ({ tipoEntidade, lista }) => {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
      {lista.map((item) => (
        <CardInfo  entidade={item} tipoEntidade={tipoEntidade}/>
      ))}
    </div>
  );
};

export default ListaCards;
