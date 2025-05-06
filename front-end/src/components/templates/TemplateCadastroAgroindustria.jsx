import React from 'react';
import FormCadastroAgroindustria from '../organisms/FormCadastroAgroindustria';

const TemplateCadastroAgroindustria = ({ onSubmit, erros }) => {
    return (
        <div className="container mx-auto pt-8 h-fit">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl font-bold text-green-dark">Cadastro de agroindústria</h1>
            </div>
            <div className="bg-white shadow-lg rounded-xl w-1/2 mx-auto py-12 mb-12 px-16 flex flex-col items-start">
                <FormCadastroAgroindustria
                    onSubmit={onSubmit}
                    erros={erros}
                />
            </div>
        </div>
    );
};

export default TemplateCadastroAgroindustria;
