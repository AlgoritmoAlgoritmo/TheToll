/*
* Author:	Iris Bermudez
* Date:		27/03/2024
*/



using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Misc.Command;



namespace Solitaire.Feedbacks {
    public class CardMovementAnimationICommand : ICommand {
        #region Variables
        private Transform transformToMove;
        private Transform targetPosition;
        private float percentageOfDistancePerFrame;
        #endregion


        #region Constructors
        public CardMovementAnimationICommand( Transform _transformToMove,
                                                Transform _targetPosition,
                                                float _percentageOfDistancePerFrame) {
            transformToMove = _transformToMove;
            targetPosition = _targetPosition;
            percentageOfDistancePerFrame = _percentageOfDistancePerFrame;
        }
        #endregion


        #region Public methods
        public async Task Execute() {
            Vector3 positionDiff = targetPosition.position - transformToMove.position;

            for( int i = 0; i <= 1f/ percentageOfDistancePerFrame; i++ ) {
               transformToMove.position += positionDiff * percentageOfDistancePerFrame;

                await Task.Yield();
            }
        }


        public IEnumerator MovementAnimation( Vector3 _targetPosition, Transform _transformToMove ) {
            bool isRunning = true;
            float speed = .01f;
            Vector3 positionDiff = _targetPosition - _transformToMove.position;


            while( isRunning ) {
                _transformToMove.position += positionDiff * speed;

                if (_transformToMove.position == _targetPosition)
                    isRunning = false;

                yield return null;
            }
        }
        #endregion
    }
}