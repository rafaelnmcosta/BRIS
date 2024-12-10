import React from 'react';
import ListaCards from '../organisms/ListaCards';

const TemplateListaVinculos = ({ vinculos }) => {
  return (
    <div className="container mx-auto pt-8 h-fit">
      <h1 className="text-2xl font-bold mb-4 text-green-dark">Lista de Vínculos</h1>
      <ListaCards tipo="Vínculo" lista={vinculos} />
    </div>
  );
};

export default TemplateListaVinculos;
