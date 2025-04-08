import React from 'react';
import BotaoPrimario from '../atoms/BotaoPrimario'

const ModalConfirmacao = ({
  open = false,
  onClose,
  onConfirm,
  title = "Confirmação",
  content = "Tem certeza que deseja realizar esta ação?",
  okText = "Confirmar",
  cancelText = "Cancelar",
  danger = false
}) => {
  return (
    open && (
      <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
        <div className="bg-white p-6 rounded-lg w-96 shadow-xl">
          <div className="flex justify-between items-center mb-4 border-b pb-2">
            <h2 className="text-xl font-bold text-green-dark">{title}</h2>
            <button 
              onClick={onClose}
              className="text-gray-500 hover:text-gray-700 text-2xl"
            >
              ×
            </button>
          </div>

          <div className="mb-6 text-gray-600">
            {content}
          </div>

          <div className="flex justify-end gap-3 mt-6">
            <button
              onClick={onClose}
              className="px-4 py-2 text-gray-600 hover:text-gray-800 transition-colors"
            >
              {cancelText}
            </button>
            <BotaoPrimario
              texto={okText}
              onClick={() => {
                onConfirm();
                onClose();
              }}
              className={`px-4 py-2 ${danger ? 'bg-red-500 hover:bg-red-600' : ''}`}
            />
          </div>
        </div>
      </div>
    )
  );
};

export default ModalConfirmacao;