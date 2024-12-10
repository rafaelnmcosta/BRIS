import React from 'react';
import ListaCards from '../organisms/ListaCards';

const TemplateLista = ({ tipo, lista }) => {
  return (
    <div className="container mx-auto pt-8 h-fit">
      <h1 className="text-2xl font-bold mb-4 text-green-dark">Lista de {tipo}s</h1>
      <ListaCards tipo={tipo} lista={lista} />
    </div>
  );
};

export default TemplateLista;
