import React from 'react';
import { Input } from 'antd';
import InputMask from 'react-input-mask';

const InputSemBordaComLabel = ({
  label,
  name,
  value,
  onChange,
  placeholder,
  icone,
  suffix,
  type = 'text',
  error,
  rows = 4,
  mask
}) => {
  const handleChange = (e) => {
    let inputValue = '';

    if (e && e.target) {
      inputValue = e.target.value;
    } else {
      inputValue = e || '';
    }

    if (typeof onChange === 'function') {
      onChange({
        target: {
          name,
          value: inputValue
        }
      });
    }
  };

  const commonProps = {
    name,
    value: value || '',
    onChange: (e) => handleChange(e),
    placeholder,
    prefix: icone,
    suffix,
    className: "border-b-2 border-t-0 border-x-0 border-green-dark hover:border-green focus:border-green focus-within:border-green"
  };

  const renderInput = () => {
    if (mask) {
      const maxDigitsFromMask = (mask) => (mask.match(/9/g) || []).length; // pega o tanto max de dígito que tem na mask pra corrigir o bug de enviar um a mais
      
      const handleMaskChange = (e) => {
        const limite = maxDigitsFromMask(mask);
        let apenasDigitos = e.target.value.replace(/\D/g, ''); // arranca tudo que não é dígito do input
        
        if (apenasDigitos.length > limite) { // se tiver mais que o limite arranca eles
          apenasDigitos = apenasDigitos.slice(0, limite);
        }
        console.log(apenasDigitos)
        handleChange({ target: { name, value: apenasDigitos } });
      };

      return (
        <InputMask
          mask={mask}
          value={value || ''}
          onChange={handleMaskChange} // usa a função auxiliar ali pra gente gerenciar as masks
          maskChar={null}
        >
          {(inputProps) => (
            <Input
              {...commonProps}
              {...inputProps}
              onChange={undefined} // garante que a handleChange não seja sobrescrita
            />
          )}
        </InputMask>
      );
    } else {
      return <Input {...commonProps} type={type} />;
    }
  };

  switch (type) {
    case 'password':
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input.Password
            {...commonProps}
          />
          {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
        </div>
      );

    case 'textarea':
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input.TextArea
            {...commonProps}
            rows={rows}
            autoSize={{ minRows: rows, maxRows: 6 }}
          />
          {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
        </div>
      );

    case 'number':
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          <Input
            {...commonProps}
            type="number"
          />
          {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
        </div>
      );

    default:
      return (
        <div className="flex flex-col mb-4">
          <label className="mb-2 text-sm font-medium text-green-dark">{label}</label>
          {renderInput()}
          {error && <span className="text-red-500 text-xs mt-1">{error}</span>}
        </div>
      );
  }
};

export default InputSemBordaComLabel;