import React from 'react';
import InputSemBorda from '../atoms/InputSemBorda';

const InputSemBordaComLabel = ({ label, placeholder, icone }) => {
  return (
    <div className="flex flex-col mb-4">
      <label className="mb-2 text-sm font-medium text-gray-700">{label}</label>
      <InputSemBorda 
        placeholder={placeholder} 
        icone={icone}
      />
    </div>
  );
};

export default InputSemBordaComLabel;
