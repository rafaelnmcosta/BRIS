import React from 'react';
import FormAutoCadastro from '../organisms/FormAutoCadastro';

const TemplateAutoCadastro = ({ handleAutoCadastro }) => {
  return (
    <div className="h-screen flex items-center justify-center">
      <FormAutoCadastro handleAutoCadastro={handleAutoCadastro} />
    </div>
  );
};

export default TemplateAutoCadastro;
