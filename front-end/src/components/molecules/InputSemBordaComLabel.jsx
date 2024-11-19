import React from 'react';
import { Input } from 'antd';

const InputSemBordaComLabel = ({ label, value, onChange, placeholder, icone, suffix, type = 'text' }) => {
  switch (type) {
    case 'password':
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input.Password
            value={value}  // Agora estamos passando o valor do estado
            onChange={onChange}  // Passando a função de onChange para atualizar o estado
            variant='' 
            type={type} 
            placeholder={placeholder} 
            prefix={icone}
            className="border-b-2 !border-green-dark hover:!border-green"
          />
        </div>
      );
  
    default:
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input
            value={value}  // Agora estamos passando o valor do estado
            onChange={onChange}  // Passando a função de onChange para atualizar o estado
            variant=''
            type={type} 
            placeholder={placeholder} 
            prefix={icone}
            suffix={suffix}
            className="border-b-2 !border-green-dark hover:!border-green"
          />
        </div>
      );
  }
};

export default InputSemBordaComLabel;
