import React from 'react';
import ListaCards from '../organisms/ListaCards';

const TemplateListaVinculos = ({ vinculos }) => {
  return (
    <div className="container mx-auto my-8">
      <h1 className="text-2xl font-bold mb-4">Lista de Vínculos</h1>
      <ListaCards tipoEntidade="Vinculo" lista={vinculos} />
    </div>
  );
};

export default TemplateListaVinculos;
